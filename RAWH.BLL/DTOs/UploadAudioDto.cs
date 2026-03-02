using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RAWH.BLL.DTOs
{
    public class UploadAudioDto
    {
        public IFormFile AudioRecord { get; set; }
    }
}
