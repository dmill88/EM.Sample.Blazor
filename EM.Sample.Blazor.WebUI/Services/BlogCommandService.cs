using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using EM.Sample.DomainModels;
using EM.Sample.DomainModels.Models;

namespace EM.Sample.Blazor.WebUI.Services
{
    public class BlogCommandService : IBlogCommandService
    {
        private readonly HttpClient _httpClient;

        public BlogCommandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BlogDto> AddBlog(BlogDto blog)
        {
            string url = "Blog/AddBlog";
            blog = await _httpClient.PostJsonAsync<BlogDto>(url, blog);
            return blog;
        }

        public async Task<BlogDto> UpdateBlog(BlogDto blog)
        {
            string url = "Blog/UpdateBlog";
            blog = await _httpClient.PutJsonAsync<BlogDto>(url, blog);
            return blog;
        }

        public async Task DeleteBlog(int id)
        {
            string url = $"Blog/DeleteBlog/{id}";
            await _httpClient.DeleteAsync(url);
        }


        public async Task<BlogPostDto> AddBlogPost(int blogId)
        {
            string url = $"BlogPosts/AddBlogPost/{blogId}";
            BlogPostDto blogPost = await _httpClient.GetJsonAsync<BlogPostDto>(url);
            return blogPost;
        }

        public async Task<BlogPostDto> AddBlogPost(BlogPostDto blogPost)
        {
            string url = $"BlogPosts/AddBlogPost";
            blogPost = await _httpClient.PostJsonAsync<BlogPostDto>(url, blogPost);
            return blogPost;
        }

        public async Task<BlogPostDto> UpdateBlogPost(BlogPostDto blogPost)
        {
            string url = $"BlogPosts/UpdatePost";
            blogPost = await _httpClient.PutJsonAsync<BlogPostDto>(url, blogPost);
            return blogPost;
        }

        public async Task DeletePost(int id)
        {
            string url = $"BlogPosts/DeletePost/{id}";
            await _httpClient.DeleteAsync(url);
        }

    }
}
