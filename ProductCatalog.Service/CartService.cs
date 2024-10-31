using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.CartDtos;
using ProductCatalog.Repository.Interfaces;
using ProductCatalog.Service.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalog.Service.BusinessLogic
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Thêm sản phẩm vào giỏ hàng (nếu đã tồn tại, tăng số lượng)
        public async Task<Cart> AddCart(AddToCartDto cartDto)
        {
            var newCart = _mapper.Map<Cart>(cartDto);
            // Kiểm tra nếu sản phẩm này đã có trong giỏ hàng của người dùng
            var existingCart = await _unitOfWork.CartRepository.FirstOrDefaultAsync(x => x.UserId == cartDto.UserId && x.ProductId == cartDto.ProductId && x.VariantId == cartDto.VariantId);

            if (existingCart != null)
            {
                // Nếu sản phẩm đã tồn tại, tăng số lượng
                existingCart.Quantity += cartDto.Quantity;
                existingCart.UpdatedAt = DateTime.Now;
                _unitOfWork.CartRepository.UpdateAsync(existingCart);
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại, thêm mới
                newCart.UserId = cartDto.UserId;
                newCart.CreatedAt = DateTime.Now;
                newCart.UpdatedAt = DateTime.Now;

                await _unitOfWork.CartRepository.AddAsync(newCart);
            }

            await _unitOfWork.CommitAsync();
            return existingCart ?? newCart;
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<bool> DeleteCart(int cartId)
        {
            var cart = await _unitOfWork.CartRepository.GetByIdAsync(cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại trong giỏ hàng");
            }

            _unitOfWork.CartRepository.Delete(cart);
            return (await _unitOfWork.CommitAsync())>0;
        }

        // Lấy giỏ hàng theo cartId
        public async Task<CartItemDto?> GetCart(int cartId)
        {
            var cart = await _unitOfWork.CartRepository.GetByIdAsync(cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại trong giỏ hàng");
            }
            cart.Product.ProductVariants = cart.Product.ProductVariants
                    .Where(v => v.VariantId == cart.VariantId).ToList();
            return _mapper.Map<CartItemDto>(cart);
        }

        // Lấy tất cả sản phẩm trong giỏ hàng của người dùng
        public async Task<List<CartItemDto>?> GetCartByUserId(int userId)
        {
            var carts = await _unitOfWork.CartRepository.GetALl()
            .Where(x => x.UserId == userId && x.Product.ProductId == x.ProductId)
            .ToListAsync();


            //if (carts == null || carts.Count == 0)
            //{
            //    throw new KeyNotFoundException("No products in cart for this user.");
            //}
            carts.ForEach(cart =>
            {
                cart.Product.ProductVariants = cart.Product.ProductVariants
                    .Where(v => v.VariantId == cart.VariantId).ToList();
            });
            return _mapper.Map<List<CartItemDto>>(carts);
        }

        // Lấy tất cả giỏ hàng (dành cho quản trị viên)
        public async Task<List<CartItemDto>> GetCarts()
        {
            var carts = await _unitOfWork.CartRepository.GetALl().ToListAsync();
            carts.ForEach(cart =>
            {
                cart.Product.ProductVariants = cart.Product.ProductVariants
                    .Where(v => v.VariantId == cart.VariantId).ToList();
            });
            return _mapper.Map<List<CartItemDto>>(carts);
        }

        // Cập nhật giỏ hàng theo cartDto (thay đổi số lượng, biến thể, ...)
        public async Task<CartItemDto> UpdateCart(CartUpdateDto cartDto)
        {
            var cart = await _unitOfWork.CartRepository.FirstOrDefaultAsync(x => x.CartId == cartDto.CartId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại trong giỏ hàng");
            }

            // Cập nhật thông tin giỏ hàng
            cart.Quantity = cartDto.Quantity;
            cart.VariantId = cartDto.VariantId;
            cart.UpdatedAt = DateTime.Now;

            _unitOfWork.CartRepository.UpdateAsync(cart);
            await _unitOfWork.CommitAsync();
            return await GetCart(cart.CartId);
        }
    }
}
