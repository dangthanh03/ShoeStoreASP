using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;
using System.Collections;
using System.Collections.Generic;

namespace ShoeStoreASP.Service.Implementation
{
    public class ShoeService : IShoeService
    {
        private readonly DatabaseContext _context;
        private readonly IFileService _fileService;

        public ShoeService(DatabaseContext context, IFileService fileService) { 
        _context = context;
            _fileService = fileService;
        }
        public Result<ShoeListViewModel> GetAllShoes(string term = "", bool paging = false, int currentPage = 0, List<int> selectedTypes = null, int BrandId=-1)
        {
            try
            {
                var shoes = _context.Shoes.ToList();
                var shoeListViewModel = new ShoeListViewModel();
                if (!string.IsNullOrEmpty(term))
                {
                    term = term.ToLower();
                    shoes = shoes.Where(a => a.Name.ToLower().StartsWith(term)).ToList();
                    shoeListViewModel.Term = term;
                   
                }
                if (selectedTypes!= null && selectedTypes.Any())
                {
                    shoes= (from shoe in shoes
                            join shoeTypes in _context.ShoeTypes on shoe.ShoeId equals shoeTypes.ShoeId
                            where selectedTypes.Contains(shoeTypes.TypeId)
                            select shoe).ToList();

                    foreach(var i in selectedTypes)
                    {
                        shoeListViewModel.Types= new List<int>();
                        shoeListViewModel.Types.Add(i);
                    }
                    shoeListViewModel.selectedTypes = string.Join(",", selectedTypes);
                }
                if (BrandId!=-1)
                {
                    shoes = (from shoe in shoes
                             where shoe.BrandId==BrandId
                             select shoe).ToList();

                    shoeListViewModel.BrandId=BrandId;
                }
                if (paging)
                {
                    int pageSize = 5;
                    int count = shoes.Count;
                    int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                    shoes = shoes.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    shoeListViewModel.PageSize = pageSize;
                    shoeListViewModel.CurrentPage = currentPage;
                    shoeListViewModel.TotalPages = TotalPages;
                }

                foreach (var par in shoes) {
                    var Brand = (from shoe in _context.Shoes
                                 join brand in _context.Brands
                                on shoe.BrandId equals brand.BrandId
                                 where shoe.ShoeId == par.ShoeId
                                 select brand.BrandName).FirstOrDefault();

                    var Types = (from shoe in _context.Shoes
                                 join shoeType in _context.ShoeTypes on shoe.ShoeId equals shoeType.ShoeId
                                 join type in _context.Types on shoeType.TypeId equals type.TypeId
                                 where shoe.ShoeId == par.ShoeId
                                 select type.TypeName

                                 ).ToList();
                    string TypeNames = string.Join(", ", Types);
                    par.BrandName = Brand.ToString();
                    par.Types = TypeNames;
                }

                shoeListViewModel.Shoes = shoes;
               


                return Result<ShoeListViewModel>.Success(shoeListViewModel);

            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Result<ShoeListViewModel>.Fail("An error occurred while fetching shoes.");
            }
        }
        public Result<Shoe> GetShoeById(int shoeId)
        {
            try
            {
                var ShoeById = _context.Shoes.FirstOrDefault(s => s.ShoeId == shoeId);

                if (ShoeById!= null)
                {
                    var Brand = (from shoe in _context.Shoes join brand in _context.Brands
                                 on shoe.BrandId equals brand.BrandId 
                                 where shoe.ShoeId==shoeId
                                 select brand.BrandName ).FirstOrDefault();

                    var Types = (from shoe in _context.Shoes
                                 join shoeType in _context.ShoeTypes on shoe.ShoeId equals shoeType.ShoeId 
                                 join type in _context.Types on shoeType.TypeId equals type.TypeId
                                 where shoe.ShoeId == shoeId
                                 select type.TypeName

                                 ).ToList();
                    string TypeNames = "";
                    foreach(var type in Types)
                    {
                        TypeNames += type + ", ";
                    }
                    TypeNames.TrimEnd( ',',' ');
                    ShoeById.Types = TypeNames;
                    ShoeById.BrandName = Brand.ToString();
                    return Result<Shoe>.Success(ShoeById);
                }
                else
                {
                    return Result<Shoe>.Fail($"Shoe with Id {shoeId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Result<Shoe>.Fail("An error occurred while fetching shoe details.");
            }
        }
        public Result<int> AddShoe(AddShoeVM model)
        {
            try
            {
                
                var newShoe = new Shoe
                {
                    Name = model.Name,
                    BrandId = model.BrandId,
                    
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    Description = model.Description
                };

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadResult = _fileService.UploadImage(model.ImageFile);

                    if (uploadResult.IsSuccess)
                    {
                        newShoe.ImageUrl = uploadResult.Data;
                    }
                    else
                    {
                        return Result<int>.Fail($"Failed to upload image: {uploadResult.Message}");
                    }
                }

                _context.Shoes.Add(newShoe);
                _context.SaveChanges();
                foreach (var type in model.Types)
                {
                    var ShoeType = new ShoeType
                    {
                        ShoeId = newShoe.ShoeId,
                        TypeId = type
                    };
                    _context.ShoeTypes.Add(ShoeType);
                    _context.SaveChanges();
                }



                return Result<int>.Success(newShoe.ShoeId);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết
                return Result<int>.Fail($"An error occurred: {ex.Message}");
            }
        }
        public Result<string> DeleteShoe(int shoeId)
        {
            try
            {
                var shoe = _context.Shoes.Find(shoeId);

                if (shoe == null)
                {
                    return Result<string>.Fail("Shoe not found");
                }

                var shoeTypesToDelete = _context.ShoeTypes.Where(st => st.ShoeId == shoeId);
                if (shoeTypesToDelete != null && shoeTypesToDelete.Any())
                {
                    _context.ShoeTypes.RemoveRange(shoeTypesToDelete);
                    _context.SaveChanges();
                }

                var deleImg = _fileService.DeleteImageByUrl(shoe.ImageUrl);
                
                _context.Shoes.Remove(shoe);
                _context.SaveChanges();

                return Result<string>.Success("Shoe deleted successfully");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết
                return Result<string>.Fail($"An error occurred: {ex.Message}");
            }
        }
        public Result<string> EditShoe(EditShoeVm model)
        {
            try
            {
                // Fetch the existing shoe from the database
                var existingShoe = _context.Shoes.FirstOrDefault(s => s.ShoeId == model.ShoeId);

                if (existingShoe == null)
                {
                    return Result<string>.Fail("Shoe not found.");
                }
                var shoeTypesToEdit = _context.ShoeTypes.Where(st => st.ShoeId == existingShoe.ShoeId);
                if (shoeTypesToEdit!= null && shoeTypesToEdit.Any())
                {
                    _context.ShoeTypes.RemoveRange(shoeTypesToEdit);
                    _context.SaveChanges();
                }
                foreach (var type in model.Types)
                {
                    var ShoeType = new ShoeType
                    {
                        ShoeId = existingShoe.ShoeId,
                        TypeId = type
                    };
                    _context.ShoeTypes.Add(ShoeType);
                    _context.SaveChanges();
                }

                // Update properties of the existing shoe
                existingShoe.Name = model.Name;
                existingShoe.BrandId = model.BrandId;
                existingShoe.Price = model.Price;
                existingShoe.StockQuantity = model.StockQuantity;
                existingShoe.Description = model.Description;

                // Save changes to the database
                _context.SaveChanges();

                return Result<string>.Success("Shoe updated successfully.");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết
                return Result<string>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public Result<string> UpdateImg(IFormFile formFile, int id)
        {
            var shoe = _context.Shoes.FirstOrDefault(s => s.ShoeId == id);

            var deleImg = _fileService.DeleteImageByUrl(shoe.ImageUrl);
            var result = _fileService.UploadImage(formFile);
            if (result.IsSuccess && deleImg.IsSuccess)
            {
              
                shoe.ImageUrl = result.Data;
                _context.SaveChanges();
                return Result<string>.Success("Change image succesfully");

            }
            else
            {
                return Result<string>.Fail("Change image fail");

            }

        }
    }
}
             