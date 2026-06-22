using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RAWH.BLL.DTOs;
using RAWH.DAL.Data;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static RAWH.DAL.Enums.AppEnums;

namespace RAWH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PneumoniaSurveyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public PneumoniaSurveyController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
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
            var lastSurvey = await _context.PneumoniaSurveyRequest
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            string childName;
            DateTime dateOfBirth;
            Gender gender;
            if (lastSurvey != null)
            {    
                childName = lastSurvey.ChildName;
                dateOfBirth = lastSurvey.DateOfBirth;
                gender = lastSurvey.Gender;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dto.ChildName) || dto.ChildName.Length > 100)
                    return BadRequest(new { message = "ChildName يجب أن يكون بين 1 و 100 حرف" });
                if (dto.DateOfBirth > DateTime.Today)
                    return BadRequest(new { message = "تاريخ الميلاد لا يمكن أن يكون في المستقبل" });
                childName = dto.ChildName;
                dateOfBirth = dto.DateOfBirth.Value;
                gender = dto.Gender.Value;
            }
            try
            {
                var survey = new PneumoniaSurveyRequest
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,

                   
                    ChildName = childName,
                    DateOfBirth = dateOfBirth,
                    Gender = gender,

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

                    var aiResponse = await httpClient.PostAsync(
                        "https://survey-api-uu1l.vercel.app/predict",
                        content
                    );

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
                    childName = survey.ChildName,
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
        public async Task<IActionResult> CreateAudio(int id, [FromForm] IFormFile audioFile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (audioFile == null || audioFile.Length == 0)
                return BadRequest(new { message = "برجاء إرسال ملف صوتي" });

            var survey = await _context.PneumoniaSurveyRequest
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (survey == null)
                return NotFound(new { message = "Survey غير موجود" });

            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio", "survey");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{audioFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            survey.AudioRecordPath = $"/uploads/audio/survey/{fileName}";

            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(120);

                // Step 1: Upload audio file
                using var formData = new MultipartFormDataContent();
                using var fileStream = audioFile.OpenReadStream();
                using var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(audioFile.ContentType);
                formData.Add(fileContent, "files", audioFile.FileName);

                var uploadResponse = await httpClient.PostAsync(
                    "https://nourhan3madd-final30.hf.space/gradio_api/upload",
                    formData
                );
                uploadResponse.EnsureSuccessStatusCode();

                var uploadJson = await uploadResponse.Content.ReadAsStringAsync();
                var uploadedPaths = JsonSerializer.Deserialize<List<string>>(uploadJson);
                var uploadedFilePath = uploadedPaths![0];

                // Step 2: Submit predict job
                var predictPayload = new
                {
                    data = new[]
                    {
                new
                {
                    path = uploadedFilePath,
                    meta = new { _type = "gradio.FileData" }
                }
            }
                };

                var predictContent = new StringContent(
                    JsonSerializer.Serialize(predictPayload),
                    Encoding.UTF8,
                    "application/json"
                );

                var predictResponse = await httpClient.PostAsync(
                    "https://nourhan3madd-final30.hf.space/gradio_api/call/predict",
                    predictContent
                );
                predictResponse.EnsureSuccessStatusCode();

                var predictJson = await predictResponse.Content.ReadAsStringAsync();
                var predictResult = JsonSerializer.Deserialize<Dictionary<string, object>>(predictJson);
                var eventId = predictResult!["event_id"].ToString();

                // Step 3: Poll for result
                string? predictionText = null;
                var pollUrl = $"https://nourhan3madd-final30.hf.space/gradio_api/call/predict/{eventId}";

                for (int i = 0; i < 60; i++)
                {
                    await Task.Delay(2000);

                    var pollResponse = await httpClient.GetAsync(pollUrl);
                    var pollBody = await pollResponse.Content.ReadAsStringAsync();

                    var lines = pollBody.Split('\n');

                    string? eventType = null;
                    string? dataContent = null;

                    foreach (var line in lines)
                    {
                        if (line.StartsWith("event:"))
                            eventType = line.Replace("event:", "").Trim();

                        if (line.StartsWith("data:"))
                            dataContent = line.Replace("data:", "").Trim();
                    }

                    if (eventType == "complete" && dataContent != null)
                    {
                        var resultArray = JsonSerializer.Deserialize<List<JsonElement>>(dataContent);
                        var rawOutput = resultArray?[0].GetString() ?? "";

                        var match = System.Text.RegularExpressions.Regex.Match(
                            rawOutput,
                            @"Prediction:\s*(Normal|Pneumonia)",
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase
                        );

                        predictionText = match.Success ? match.Groups[1].Value : rawOutput;
                        break;
                    }

                    if (eventType == "error")
                    {
                        predictionText = "Error";
                        break;
                    }
                }

                // Update الـ survey
                survey.AudioRiskPrediction = predictionText ?? "Unknown";
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Audio prediction completed successfully",
                    audioRecordPath = survey.AudioRecordPath,
                    audioRiskPrediction = survey.AudioRiskPrediction
                });
            }
            catch (Exception ex)
            {
                // لو في error في الـ AI برضو نحفظ الـ path
                await _context.SaveChangesAsync();

                return StatusCode(500, new
                {
                    message = "حدث خطأ أثناء معالجة الملف الصوتي",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


        [HttpGet("{id}/final-diagnosis")]
        public async Task<IActionResult> GetFinalDiagnosis(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var survey = await _context.PneumoniaSurveyRequest
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (survey == null)
                return NotFound(new { message = "Survey غير موجود" });

            if (string.IsNullOrEmpty(survey.RiskPrediction) || survey.RiskPrediction == "Error")
                return BadRequest(new { message = "نتيجة الاستبيان غير متاحة" });

            if (string.IsNullOrEmpty(survey.AudioRiskPrediction) || survey.AudioRiskPrediction == "Error")
                return BadRequest(new { message = "نتيجة التسجيل الصوتي غير متاحة" });

            var finalResult = FinalDiagnosis(survey.RiskPrediction, survey.AudioRiskPrediction);

            survey.FinalDiagnosis = finalResult;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = survey.Id,
                riskPrediction = survey.RiskPrediction,
                audioRiskPrediction = survey.AudioRiskPrediction,
                finalDiagnosis = survey.FinalDiagnosis
            });
        }







        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var surveys = await _context.PneumoniaSurveyRequest
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var result = surveys.Select(x => new
            {
                FinalDiagnosis = TranslateDiagnosis(x.FinalDiagnosis),
                Time = FormatTime(x.CreatedAt),
                Day = GetDayLabel(x.CreatedAt)
            });

            return Ok(result);
        }


        [HttpGet("chart/sequence")]
        public async Task<IActionResult> GetSequenceChart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var surveys = await _context.PneumoniaSurveyRequest
                .Where(x => x.UserId == userId && x.FinalDiagnosis != null)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            if (!surveys.Any())
                return Ok(new { message = "No data" });

            int index = 1;

            var chartData = surveys.Select(x => new
            {
                x = index++,
                y = MapDiagnosisToValue(x.FinalDiagnosis),
                label = TranslateDiagnosis(x.FinalDiagnosis)
            });
            var childName = surveys.First().ChildName;
            return Ok(new
            {
                childName = childName,
                data = chartData
            });
        }





        //Helpers

        private string FormatTime(DateTime date)
        {
            return date.ToLocalTime().ToString("hh:mm tt");
        }

        private string GetDayLabel(DateTime date)
        {
            var localDate = date.ToLocalTime().Date;
            var today = DateTime.Now.Date;

            if (localDate == today)
                return "اليوم";

            if (localDate == today.AddDays(-1))
                return "أمس";

            return localDate.ToString("d/M");
        }


        private string FinalDiagnosis(string surveyResult, string audioResult)
        {
            if (audioResult == "Normal")
            {
                if (surveyResult == "Low Risk")
                    return "Low Risk";

                else if (surveyResult == "Moderate Risk")
                    return "Low Risk";

                else if (surveyResult == "High Risk")
                    return "Moderate Risk";

                else if (surveyResult == "Severe Pneumonia")
                    return "Severe Pneumonia";
            }
            else if (audioResult == "Pneumonia")
            {
                if (surveyResult == "Low Risk")
                    return "Moderate Risk";

                else if (surveyResult == "Moderate Risk")
                    return "High Risk";

                else if (surveyResult == "High Risk")
                    return "Severe Pneumonia";

                else if (surveyResult == "Severe Pneumonia")
                    return "Severe Pneumonia";
            }

            return "Unknown";
        }


        private string TranslateDiagnosis(string? diagnosis)
        {
            return diagnosis switch
            {
                "Low Risk" => "جيدة",
                "Moderate Risk" => "متوسطة",
                "High Risk" => "سيئة",
                "Severe Pneumonia" => "سيئة جدا",
                _ => "غير محدد"
            };
        }



        private int MapDiagnosisToValue(string diagnosis)
        {
            return diagnosis switch
            {
                "Low Risk" => 1,
                "Moderate Risk" => 2,
                "High Risk" => 3,
                "Severe Pneumonia" => 4,
                _ => 0
            };
        }
    }
}