﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Dtos;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Product;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly IProductService _productService;
        private readonly IFileServices _fileServices;

        public ProductController
            (
            ShopDbContext context,
            IProductService productService,
            IFileServices fileServices
            )
        {
            _context = context;
            _productService = productService;
            _fileServices = fileServices;
        }

        //ListItem
        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Product
                .OrderByDescending(y => y.CreatedAt)
                .Select(x => new ProductListItem
                {
                    Id = x.Id,
                    Brand = x.Brand,
                    ModelName = x.ModelName,
                    Price = x.Price,
                    Color = x.Color,
                    Transmission = x.Transmission,
                    Power = x.Power,
                    Year = x.Year
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ProductViewModel model = new ProductViewModel();

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            var dto = new ProductDto()
            {
                Id = model.Id,
                Brand = model.Brand,
                ModelName = model.ModelName,
                Price = model.Price,
                Color = model.Color,
                Transmission = model.Transmission,
                Power = model.Power,
                Year = model.Year,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Files = model.Files,
                ExistingFilePaths = model.ExistingFilePaths.Select(x => new ExistingFilePathDto
                {
                    Id = x.PhotoId,
                    ExistingFilePath = x.FilePath,
                    ProductId = x.ProductId
                }).ToArray()
            };

            var result = await _productService.Add(dto);

            if(result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productService.Delete(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productService.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var photos = await _context.ExistingFilePath
                .Where(x => x.ProductId == id)
                .Select(y => new ExistingFilePathViewModel
                {
                    FilePath = y.FilePath,
                    PhotoId = y.Id
                })
                .ToArrayAsync();

            var model = new ProductViewModel();

            model.Id = product.Id;
            model.Brand = product.Brand;
            model.ModelName = product.ModelName;
            model.Price = product.Price;
            model.Color = product.Color;
            model.Transmission = product.Transmission;
            model.Power = product.Power;
            model.Year = product.Year;

            model.ModifiedAt = product.ModifiedAt;
            model.CreatedAt = product.CreatedAt;
            model.ExistingFilePaths.AddRange(photos);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var dto = new ProductDto()
            {
                Id = model.Id,
                Brand = model.Brand,
                ModelName = model.ModelName,
                Price = model.Price,
                Color = model.Color,
                Transmission = model.Transmission,
                Power = model.Power,
                Year = model.Year,

                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Files = model.Files,
                ExistingFilePaths = model.ExistingFilePaths
                    .Select(x => new ExistingFilePathDto
                    {
                        Id = x.PhotoId,
                        ExistingFilePath = x.FilePath,
                        ProductId = x.ProductId
                    }).ToArray()
            };

            var result = await _productService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveImage(ExistingFilePathViewModel model)
        {
            var dto = new ExistingFilePathDto()
            {
                Id = model.PhotoId
            };

            var photo = await _fileServices.RemoveImage(dto);
            if (photo == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
