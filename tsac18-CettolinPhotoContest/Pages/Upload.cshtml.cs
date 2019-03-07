using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tsac18_CettolinPhotoContest.data;

namespace tsac18_CettolinPhotoContest.web.Pages
{
    [Authorize]
    public class UploadModel : PageModel
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly IDataAccess _data;
        private readonly UserManager<IdentityUser> _userManager;

        public UploadModel(IDataAccess data, UserManager<IdentityUser> userManager, IAmazonS3 amazonS3)
        {
            _userManager = userManager;
            _data = data;
            _amazonS3 = amazonS3;
        }

        [BindProperty]
        public IFormFile photoUpload { get; set; }
        
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost() //mancano try e catch
        {
            using (var memorystream = new MemoryStream())
            {
                 
                photoUpload.CopyTo(memorystream);
                var fileTransferUtility =  new TransferUtility(_amazonS3);

                // Option 4. Specify advanced settings.
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = "tsac2018-cettolins3",
                    InputStream = memorystream,
                    Key = photoUpload.FileName,
                    CannedACL = S3CannedACL.PublicRead
                };
                
                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                var UserId = _userManager.GetUserId(User);
                string url = "https://d24ewcetjs2g2j.cloudfront.net/" + photoUpload.FileName;
                _data.LoadImageS3(url,UserId);
               
            }

            return RedirectToPage("./Index");
        }

    }
}

//public static void Main()
//{
//    s3Client = new AmazonS3Client(bucketRegion);
//    UploadFileAsync().Wait();
//}

//private static async Task<IActionResult> UploadFileAsync()
//{
//    try
//    {
//        var fileTransferUtility =
//            new TransferUtility(s3Client);

//        // Option 4. Specify advanced settings.
//        var fileTransferUtilityRequest = new TransferUtilityUploadRequest
//        {
//            BucketName = bucketName,
//            CannedACL = S3CannedACL.PublicRead
//        };
//        fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
//        fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

//        await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
//        Console.WriteLine("Upload 4 completed");
//        return RedirectToPage("/index");
//    }
//    catch (AmazonS3Exception e)
//    {
//        Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
//        return RedirectToPage("/index");
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
//        return RedirectToPage("/index");
//    }

//}
//}
//}
//}