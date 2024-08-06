using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories.Product;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IStorageService _storageService;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductReadRepository _productReadRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository)
        {
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.Files);

            E.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(z => new E.ProductImageFile
            {
                FileName = z.fileName,
                Path = z.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new E.Product[] { product }
            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
