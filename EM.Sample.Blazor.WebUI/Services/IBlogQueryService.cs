using EM.Data.Helpers;
using EM.Sample.DomainModels.Enums;
using EM.Sample.DomainModels.Filters;
using EM.Sample.DomainModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EM.Sample.Blazor.WebUI.Services
{
    public interface IBlogQueryService
    {
        Task<IEnumerable<PostListItemDto>> GetAllBlogPosts(int id);
        Task<IEnumerable<AuthorDto>> GetAuthors();
        Task<BlogDto> GetBlog(int id);
        Task<BlogPostDto> GetBlogPost(int id);
        Task<PagedDataResult> GetBlogPosts(BlogPostsFilter filter);
        Task<PagedDataResult> GetBlogPosts(int blogId, int skip = 0, int take = 10, string title = null);
        Task<IEnumerable<BlogListItemDto>> GetBlogs(BlogStatuses status = BlogStatuses.Published, int? primaryAuthorId = null);
        IEnumerable<ListItem<int>> GetBlogStatusList();
        IEnumerable<ListItem<int>> GetCommentStatusesList();
        IEnumerable<ListItem<int>> GetPostStatusesList();
        Task<IEnumerable<TagDto>> GetBlogTags(int blogId);
        Task<IEnumerable<TagDto>> GetPostTags(int postId);
        Task<IEnumerable<TagDto>> GetTags();
        Task<IEnumerable<TagDto>> GetUnusedBlogTags(int blogId);
        Task<IEnumerable<TagDto>> GetUnusedPostTags(int postId);
        IEnumerable<ListItem<int>> GetUserRolesList();
    }
}