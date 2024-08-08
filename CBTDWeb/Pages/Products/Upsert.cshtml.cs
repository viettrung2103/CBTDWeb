using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CBTDWeb.Pages.Products
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Product objProduct { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> ManufacturerList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            objProduct = new Product();
            CategoryList = new List<SelectListItem>();
            ManufacturerList = new List<SelectListItem>();
        }

        public IActionResult OnGet(int? id)
        {
            objProduct = new Product();
            CategoryList = _unitOfWork.Category.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }
                // the result of the Select querry is saved in c parameter, which is initiated as a new List item
                );
            ManufacturerList = _unitOfWork.Manufacturer.GetAll()
                .Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                }
                );

            if (id == null || id == 0) //create mode
            {
                return Page();
            }

            //edit mode

            if (id != 0)  //retreive product from DB
            {
                objProduct = _unitOfWork.Product.GetById(id);
            }

            if (objProduct == null) //maybe nothing returned
            {
                return NotFound();
            }
            return Page();
        }




        public IActionResult OnPost()
        {
                // getting the path root of the environment and save it to 
            string webRootPath = _webHostEnvironment.WebRootPath; // eg: C:/, D:/
            //Form.Files[] array enctype="multipart/form-data"
            var files = HttpContext.Request.Form.Files; // it return number of files
            // if number of files > 0, image url is uploaded, if == 0, no file is uploaded

            // Check if the product is new (create)
            if (objProduct.Id == 0)
            {
                // Check if an image was uploaded
                if (files.Count > 0)
                {
                    // Generate a unique identifier for the image name
                    // Guid = global unuige id == almost guaranteed a unuique id
                    string fileName = Guid.NewGuid().ToString();

                    // Define the path to store the uploaded image
                    // the @ to keep the whole path as it is
                    
                    var uploads = Path.Combine(webRootPath, @"images\products\"); // D:\images\products\

                    // Get the file extension
                    // when upload, there is only 1 item in the file array
                    var extension = Path.GetExtension(files[0].FileName);

                    // Create the full path for the uploaded image
                    var fullPath = Path.Combine(uploads, fileName + extension);

                    // Save the uploaded image to the server
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    // Set the URL path for the image in the database
                    objProduct.ImageUrl = @"\images\products\" + fileName + extension;
                }

                // Add the new product to the database
                _unitOfWork.Product.Add(objProduct);
            }
            else
            {
                // Retrieve the existing product from the database
                var objProductFromDb = _unitOfWork.Product.Get(p => p.Id == objProduct.Id);

                // Check if an image was uploaded
                if (files.Count > 0)
                {
                    // Generate a unique identifier for the new image name
                    string fileName = Guid.NewGuid().ToString();

                    // Define the path to store the uploaded image
                    var uploads = Path.Combine(webRootPath, @"images\products\");

                    // Get the file extension
                    var extension = Path.GetExtension(files[0].FileName);

                    // Delete the existing image associated with the product
                    if (objProductFromDb.ImageUrl != null)
                    {
                        // when getting the imageUrl from the database, it will start with \\ >> trim it
                        var imagePath = Path.Combine(webRootPath, objProduct.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // Create the full path for the new uploaded image
                    var fullPath = Path.Combine(uploads, fileName + extension);

                    // Save the new uploaded image to the server
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    // Set the URL path for the new image in the database
                    objProduct.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    // Update the existing product's image URL
                    objProductFromDb.ImageUrl = objProduct.ImageUrl;
                }

                // Update the existing product in the database
                _unitOfWork.Product.Update(objProduct);
            }

            // Save changes to the database
            _unitOfWork.Commit();

            // Redirect to the Products Page
            return RedirectToPage("./Index");
        }

    }
}
