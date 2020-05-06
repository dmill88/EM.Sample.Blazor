using EM.Sample.DomainModels.Models;
using System.Threading.Tasks;

namespace EM.Sample.Blazor.WebUI.Services
{
    public interface IBlogCommandService
    {
        Task<BlogDto> AddBlog(BlogDto blog);
        Task<BlogPostDto> AddBlogPost(BlogPostDto blogPost);
        Task<BlogPostDto> AddBlogPost(int blogId);
        Task DeleteBlog(int id);
        Task DeletePost(int id);
        Task<BlogDto> UpdateBlog(BlogDto blog);
        Task<BlogPostDto> UpdateBlogPost(BlogPostDto blogPost);
    }
}