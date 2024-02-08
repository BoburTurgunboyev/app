using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Domain.Models;
using MyProject.Infrastructure.Repositories.ProductRepo;
using MyProject.Services.ExtentionFunctions;
using MyProject.Services.Services.ProductServices;
using MyProject.ViewModels;

namespace MyProject.Controllers
{
   
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly VatCalculator _vatCalculator;

        public ProductController(IProductService productService, VatCalculator vatCalculator, IProductRepository productRepository, UserManager<User> userManager)
        {
            _productService = productService;
            _vatCalculator = vatCalculator;
            _productRepository = productRepository;
            _userManager = userManager;
        }




        // Get: Products

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProduct();
            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Quantiy = p.Quantiy,
                Price = p.Price,
                TotalPriceWithVAT = _vatCalculator.CalculateTotalPriceWithVat(p.Quantiy, p.Price),
            }).ToList();

            return View(productViewModels);

        }

        // Get: Product/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _productService.GetByIdProduct(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);

        }

        // Get: Product/Create
        public IActionResult Create()
        {
            return View();
        }


        // Post: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Title,Quantiy,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                await _productService.CreateProduct(product);
                await _productRepository.CreateAudit(product, null, "Create",user);

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Get: Users/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Post: Users/Edit/

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Quantiy,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var oldProduct = await _productRepository.GetOldValueAsync(id);
                var newProduct = await _productService.UpdateProduct(id, product);
                await _productRepository.CreateAudit(oldProduct, newProduct, "Edit", user);


                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }



        //   Get:  Product/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdProduct(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // Post: Product/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
