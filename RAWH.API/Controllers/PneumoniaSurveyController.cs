using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAWH.BLL.DTOs;
using RAWH.DAL.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RAWH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PneumoniaSurveyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PneumoniaSurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Create Survey =====
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PneumoniaSurveyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(dto.ChildName) || dto.ChildName.Length > 100)
                return BadRequest(new { message = "ChildName يجب أن يكون بين 1 و 100 حرف" });

            if (dto.DateOfBirth > DateTime.Today)
                return BadRequest(new { message = "تاريخ الميلاد لا يمكن أن يكون في المستقبل" });

            try
            {
                var survey = new PneumoniaSurveyRequest
                {
                    UserId = userId,
                    ChildName = dto.ChildName,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,

                    FeverDuration = dto.FeverDuration,
                    FeverLevel = dto.FeverLevel,
                    FeverResponse = dto.FeverResponse,

                    CoughTime = dto.CoughTime,
                    CoughType = dto.CoughType,
                    PhlegmStatus = dto.PhlegmStatus,
                    CoughSeverity = dto.CoughSeverity,

                    HasAbnormalBreathingSound = dto.HasAbnormalBreathingSound,
                    BreathingEffort = dto.BreathingEffort,
                    FeedingAbility = dto.FeedingAbility,
                    HasChestIndrawing = dto.HasChestIndrawing,

                    HasNasalFlaring = dto.HasNasalFlaring,
                    HasCyanosis = dto.HasCyanosis,

                    FatigueStatus = dto.FatigueStatus,
                    AppetiteStatus = dto.AppetiteStatus,

                    HasWeakCry = dto.HasWeakCry,
                    HasSevereRunnyNoseWithBreathingDifficulty = dto.HasSevereRunnyNoseWithBreathingDifficulty,

                    RecurrentChestIssues = dto.RecurrentChestIssues,
                    HeartCondition = dto.HeartCondition
                };

                survey.CalculateAge();

                _context.PneumoniaSurveyRequest.Add(survey);
                await _context.SaveChangesAsync();

                try
                {
                    using var httpClient = new HttpClient();

                    var aiDto = new
                    {
                        FeverDuration = dto.FeverDuration.ToString(),
                        FeverLevel = dto.FeverLevel.ToString(),
                        FeverResponse = dto.FeverResponse.ToString(),

                        CoughTime = dto.CoughTime.ToString(),
                        CoughType = dto.CoughType.ToString(),
                        PhlegmStatus = dto.PhlegmStatus.ToString(),
                        CoughSeverity = dto.CoughSeverity.ToString(),

                        HasAbnormalBreathingSound = dto.HasAbnormalBreathingSound,
                        BreathingEffort = dto.BreathingEffort.ToString(),
                        FeedingAbility = dto.FeedingAbility.ToString(),
                        HasChestIndrawing = dto.HasChestIndrawing.ToString(),

                        HasNasalFlaring = dto.HasNasalFlaring,
                        HasCyanosis = dto.HasCyanosis,

                        FatigueStatus = dto.FatigueStatus,
                        AppetiteStatus = dto.AppetiteStatus.ToString(),

                        HasWeakCry = dto.HasWeakCry,
                        HasSevereRunnyNoseWithBreathingDifficulty = dto.HasSevereRunnyNoseWithBreathingDifficulty,

                        RecurrentChestIssues = dto.RecurrentChestIssues.ToString(),
                        HeartCondition = dto.HeartCondition.ToString()
                    };

                    var aiRequest = JsonSerializer.Serialize(aiDto);
                    var content = new StringContent(aiRequest, Encoding.UTF8, "application/json");

                    var aiResponse = await httpClient.PostAsync("https://survey-api-uu1l.vercel.app/predict", content);
                    aiResponse.EnsureSuccessStatusCode();

                    var resultJson = await aiResponse.Content.ReadAsStringAsync();
                    var aiResult = JsonSerializer.Deserialize<Dictionary<string, object>>(resultJson);

                    survey.RiskPrediction = aiResult["result"].ToString();
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    survey.RiskPrediction = "Error";
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    message = "Survey submitted successfully",
                    id = survey.Id,
                    riskPrediction = survey.RiskPrediction
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "حدث خطأ أثناء حفظ البيانات",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPost("{id}/upload-audio")]
        public async Task<IActionResult> UploadAudio(int id, [FromForm] UploadAudioDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var survey = await _context.PneumoniaSurveyRequest
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (survey == null)
                return NotFound();

            if (dto.AudioRecord == null || dto.AudioRecord.Length == 0)
                return BadRequest("No file");

            var allowedExtensions = new[] { ".mp3", ".wav", ".m4a" };
            var ext = Path.GetExtension(dto.AudioRecord.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Invalid file type");

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "audio",
                "survey"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.AudioRecord.CopyToAsync(stream);
            }

            survey.AudioRecordPath = $"/uploads/audio/survey/{fileName}";
            await _context.SaveChangesAsync();

            // ✅ التصليح هنا: AudioRiskPrediction بدل RiskPrediction + URL صح
            try
            {
                using var httpClient = new HttpClient();

                var aiDto = new
                {
                    AudioPath = survey.AudioRecordPath
                };

                var aiRequest = JsonSerializer.Serialize(aiDto);
                var content = new StringContent(aiRequest, Encoding.UTF8, "application/json");

                var aiResponse = await httpClient.PostAsync("https://survey-api-uu1l.vercel.app/predict-audio", content);
                aiResponse.EnsureSuccessStatusCode();

                var resultJson = await aiResponse.Content.ReadAsStringAsync();
                var aiResult = JsonSerializer.Deserialize<Dictionary<string, object>>(resultJson);

                survey.AudioRiskPrediction = aiResult["result"].ToString(); // ✅ AudioRiskPrediction
                await _context.SaveChangesAsync();
            }
            catch
            {
                survey.AudioRiskPrediction = "AudioAnalysisError"; // ✅ AudioRiskPrediction
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                message = "Audio uploaded and analyzed",
                audioPath = survey.AudioRecordPath,
                audioRiskPrediction = survey.AudioRiskPrediction // ✅ AudioRiskPrediction
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurvey(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var survey = await _context.PneumoniaSurveyRequest
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (survey == null)
                return NotFound();

            var response = new PneumoniaSurveyGet_PutDto
            {
                FeverDuration = survey.FeverDuration,
                FeverLevel = survey.FeverLevel,
                FeverResponse = survey.FeverResponse,

                CoughTime = survey.CoughTime,
                CoughType = survey.CoughType,
                PhlegmStatus = survey.PhlegmStatus,
                CoughSeverity = survey.CoughSeverity,

                HasAbnormalBreathingSound = survey.HasAbnormalBreathingSound,
                BreathingEffort = survey.BreathingEffort,
                FeedingAbility = survey.FeedingAbility,
                HasChestIndrawing = survey.HasChestIndrawing,

                HasNasalFlaring = survey.HasNasalFlaring,
                HasCyanosis = survey.HasCyanosis,

                FatigueStatus = survey.FatigueStatus,
                AppetiteStatus = survey.AppetiteStatus,

                HasWeakCry = survey.HasWeakCry,
                HasSevereRunnyNoseWithBreathingDifficulty = survey.HasSevereRunnyNoseWithBreathingDifficulty,

                RecurrentChestIssues = survey.RecurrentChestIssues,
                HeartCondition = survey.HeartCondition
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PneumoniaSurveyGet_PutDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var survey = await _context.PneumoniaSurveyRequest
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (survey == null)
                return NotFound();

            survey.FeverDuration = dto.FeverDuration;
            survey.FeverLevel = dto.FeverLevel;
            survey.FeverResponse = dto.FeverResponse;

            survey.CoughTime = dto.CoughTime;
            survey.CoughType = dto.CoughType;
            survey.PhlegmStatus = dto.PhlegmStatus;
            survey.CoughSeverity = dto.CoughSeverity;

            survey.HasAbnormalBreathingSound = dto.HasAbnormalBreathingSound;
            survey.BreathingEffort = dto.BreathingEffort;
            survey.FeedingAbility = dto.FeedingAbility;
            survey.HasChestIndrawing = dto.HasChestIndrawing;

            survey.HasNasalFlaring = dto.HasNasalFlaring;
            survey.HasCyanosis = dto.HasCyanosis;

            survey.FatigueStatus = dto.FatigueStatus;
            survey.AppetiteStatus = dto.AppetiteStatus;

            survey.HasWeakCry = dto.HasWeakCry;
            survey.HasSevereRunnyNoseWithBreathingDifficulty = dto.HasSevereRunnyNoseWithBreathingDifficulty;

            survey.RecurrentChestIssues = dto.RecurrentChestIssues;
            survey.HeartCondition = dto.HeartCondition;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Survey updated successfully" });
        }
    }
}