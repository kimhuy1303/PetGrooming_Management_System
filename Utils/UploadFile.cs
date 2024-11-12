using static Azure.Core.HttpHeader;

namespace PetGrooming_Management_System.Utils
{
    public class UploadFile
    {
        private static readonly IWebHostEnvironment? _hostEnvironment;
        public static string GetCurrentDirectory()
        {
            var result = Directory.GetCurrentDirectory();
            return result;
        }
        public static string GetStaticContentDirectory()
        {
            var result = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\");
            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }
            return result;
        }
        public static string GetFilePath(string FileName)
        {
            var _GetStaticContentDirectory = GetStaticContentDirectory();
            var result = Path.Combine(_GetStaticContentDirectory, FileName);
            return result;
        }
        public static void DeleteImage(string imageName)
        {
            var imagePath = GetFilePath(imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
        public static string UploadFilePath(IFormFile file)
        {
            string filename = "";
            try
            {
                FileInfo fileinfo = new FileInfo(file.FileName);
                filename = file.FileName;
                var getFilePath = GetFilePath(filename);

                using (var stream = new FileStream(getFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return filename;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
