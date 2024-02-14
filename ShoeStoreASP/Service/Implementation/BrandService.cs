using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;
using System;

namespace ShoeStoreASP.Service.Implementation
{
    public class BrandService: IBrandService
    {
        private readonly DatabaseContext _context;
        public BrandService(DatabaseContext context)
        {
            _context = context;
        }

        public Result<Brand> Add(Brand brand)
        {
            try
            {
                _context.Brands.Add(brand);
                _context.SaveChanges();

                return Result<Brand>.Success(brand);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return Result<Brand>.Fail($"An error occurred while adding brand: {ex.Message}");
            }
        }

        public Result<BrandListVm> GetAllBrand()
        {
            try
            {
                var Brands = _context.Brands.ToList();
            var BrandVm = new BrandListVm { Brands = Brands};
                return  Result<BrandListVm>.Success(BrandVm);
            }
            catch (Exception e) {
                return Result<BrandListVm>.Fail("An error occurred while fetching shoes.");
            }
          
        }

        public Result<Brand> GetBrandById(int brandId)
        {
            try
            {
                var brand = _context.Brands.Find(brandId);
                if (brand != null)
                {
                    return Result<Brand>.Success(brand);
                }
                else
                {
                    return Result<Brand>.Fail($"Brand with ID {brandId} not found");
                }
            }
            catch (Exception ex)
            {
                return Result<Brand>.Fail($"An error occurred while fetching brand: {ex.Message}");
            }
        }

        public Result<Brand> UpdateBrand(Brand brand)
        {
            try
            {
                _context.Entry(brand).State = EntityState.Modified;
                _context.SaveChanges();

                return Result<Brand>.Success(brand);
            }
            catch (Exception ex)
            {
                return Result<Brand>.Fail($"An error occurred while updating brand: {ex.Message}");
            }
        }

        public Result<Brand> DeleteBrand(int brandId)
        {
            try
            {
                var brand = _context.Brands.Find(brandId);
                if (brand != null)
                {
                    _context.Brands.Remove(brand);
                    _context.SaveChanges();

                    return Result<Brand>.Success(brand);
                }
                else
                {
                    return Result<Brand>.Fail($"Brand with ID {brandId} not found");
                }
            }
            catch (Exception ex)
            {
                return Result<Brand>.Fail($"An error occurred while deleting brand: {ex.Message}");
            }
        }




    }
}
