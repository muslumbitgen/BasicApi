using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using BasicApi.Items.Dtos;
using BasicApi.Items.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public class ImageService : ServiceBase, IImageService
    {
        private IOptions<ImageOptions> _options { get; }
        private Cloudinary _cloudinary;

        public ImageService(IOptions<ImageOptions> options)
        {
            _options = options;
            Account account = new Account(_options.Value.CloudName, _options.Value.ApiKey, _options.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> SaveAsync(string imageUrl)
        {
            try
            {

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(@imageUrl)
                };
                var uploadResult = _cloudinary.Upload(uploadParams);
                return uploadResult.Url.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
