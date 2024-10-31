using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service.BusinessLogic.Interfaces
{
    public interface ICartService
    {
        // Thêm sản phẩm vào giỏ hàng của người dùng
        Task<Cart> AddCart(AddToCartDto cartDto);

        // Xóa sản phẩm khỏi giỏ hàng
        Task<bool> DeleteCart(int cartId);

        // Lấy giỏ hàng dựa trên CartId
        Task<CartItemDto> GetCart(int cartId);

        // Lấy tất cả sản phẩm trong giỏ hàng của người dùng
        Task<List<CartItemDto>?> GetCartByUserId(int userId);

        // Lấy danh sách tất cả các giỏ hàng (cho quản trị viên)
        Task<List<CartItemDto>> GetCarts();

        // Cập nhật thông tin sản phẩm trong giỏ hàng (ví dụ: số lượng)
        Task<CartItemDto> UpdateCart(CartUpdateDto cartDto);
    }
}
