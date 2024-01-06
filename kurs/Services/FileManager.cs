namespace kurs.Services
{
    public class FileManager
    {
        private readonly IWebHostEnvironment _env;

        public FileManager(IWebHostEnvironment env) => _env = env;

        public string UploadFile(IFormFile file, string directoryName)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = $"\\uploads\\{directoryName}\\{fileName}";
            using (var fileStream = new FileStream(_env.WebRootPath + filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return filePath;
        }

    }
}
