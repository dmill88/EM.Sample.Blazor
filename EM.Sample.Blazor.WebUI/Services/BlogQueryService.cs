using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using EM.Sample.DomainModels.Enums;
using EM.Sample.DomainModels.Models;
using EM.Sample.DomainModels.Filters;
using EM.Data.Helpers;
using System.Diagnostics;

namespace EM.Sample.Blazor.WebUI.Services
{
    public class BlogQueryService : IBlogQueryService
    {
        private readonly HttpClient _httpClient;

        public BlogQueryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async public Task<IEnumerable<AuthorDto>> GetAuthors()
        {
            var authors = await _httpClient.GetJsonAsync<IEnumerable<AuthorDto>>("AuthorQueries/GetAuthors");
            return authors;
        }

        public IEnumerable<ListItem<int>> GetBlogStatusList()
        {
            IEnumerable<ListItem<int>> items = typeof(BlogStatuses).GetItemsAsInteger();
            return items;
        }

        public IEnumerable<ListItem<int>> GetCommentStatusesList()
        {
            IEnumerable<ListItem<int>> items = typeof(CommentStatuses).GetItemsAsInteger();
            return items;
        }

        public IEnumerable<ListItem<int>> GetPostStatusesList()
        {
            IEnumerable<ListItem<int>> items = typeof(PostStatuses).GetItemsAsInteger();
            return items;
        }

        public IEnumerable<ListItem<int>> GetUserRolesList()
        {
            IEnumerable<ListItem<int>> items = typeof(UserRoles).GetItemsAsInteger();
            return items;
        }

        public async Task<IEnumerable<BlogListItemDto>> GetBlogs(BlogStatuses status = BlogStatuses.Published, int? primaryAuthorId = null)
        {
            string url = $"BlogQueries/GetBlogs?statusId={(int)status}";
            if (primaryAuthorId.HasValue)
            {
                url += $"&primaryAuthorId={primaryAuthorId.Value}";
            }
            var blogs = await _httpClient.GetJsonAsync<IEnumerable<BlogListItemDto>>(url);
            if (blogs.Count() > 0)
            {
                Debug.WriteLine("blog " + blogs.First().Name);
            }
            return blogs;
        }

        public async Task<PagedDataResult> GetBlogPosts(int blogId, int skip = 0, int take = 10, string title = null)
        {
            string url = $"BlogQueries/GetBlogPosts";
            BlogPostsFilter filter = new BlogPostsFilter() { BlogId = blogId, Skip = skip, Take = take };
            if (!string.IsNullOrWhiteSpace(title))
            {
                filter.Title = title;
            }
            PagedDataResult pagedData = await _httpClient.PostJsonAsync<PagedDataResult>(url, filter);
            return pagedData;
        }

        public async Task<PagedDataResult> GetBlogPosts(BlogPostsFilter filter)
        {
            string url = $"BlogQueries/GetBlogPosts";
            PagedDataResult pagedData = await _httpClient.PostJsonAsync<PagedDataResult>(url, filter);
            return pagedData;
        }

        public async Task<BlogDto> GetBlog(int id)
        {
            string url = $"BlogQueries/GetBlog/{id}";
            BlogDto blog = await _httpClient.GetJsonAsync<BlogDto>(url);
            return blog;
        }

        public async Task<BlogPostDto> GetBlogPost(int id)
        {
            string url = $"BlogQueries/GetBlogPost/{id}";
            BlogPostDto blogPost = await _httpClient.GetJsonAsync<BlogPostDto>(url);
            return blogPost;
        }

        public async Task<IEnumerable<PostListItemDto>> GetAllBlogPosts(int id)
        {
            string url = $"BlogQueries/GetAllBlogPosts?blogId={id}";
            IEnumerable<PostListItemDto> blogPosts = await _httpClient.GetJsonAsync<IEnumerable<PostListItemDto>>(url);
            return blogPosts;
        }

        public async Task<IEnumerable<TagDto>> GetTags()
        {
            string url = $"BlogQueries/GetTags";
            IEnumerable<TagDto> tags = await _httpClient.GetJsonAsync<IEnumerable<TagDto>>(url);
            return tags;
        }


        public async Task<IEnumerable<TagDto>> GetBlogTags(int blogId)
        {
            string url = $"BlogQueries/GetBlogTags/{blogId}";
            IEnumerable<TagDto> tags = await _httpClient.GetJsonAsync<IEnumerable<TagDto>>(url);
            return tags;
        }

        public async Task<IEnumerable<TagDto>> GetUnusedBlogTags(int blogId)
        {
            string url = $"BlogQueries/GetUnusedBlogTags/{blogId}";
            IEnumerable<TagDto> tags = await _httpClient.GetJsonAsync<IEnumerable<TagDto>>(url);
            return tags;
        }

        public async Task<IEnumerable<TagDto>> GetPostTags(int postId)
        {
            string url = $"BlogQueries/GetPostTags/{postId}";
            IEnumerable<TagDto> tags = await _httpClient.GetJsonAsync<IEnumerable<TagDto>>(url);
            return tags;
        }

        public async Task<IEnumerable<TagDto>> GetUnusedPostTags(int postId)
        {
            string url = $"BlogQueries/GetUnusedPostTags/{postId}";
            IEnumerable<TagDto> tags = await _httpClient.GetJsonAsync<IEnumerable<TagDto>>(url);
            return tags;
        }
    }
}
