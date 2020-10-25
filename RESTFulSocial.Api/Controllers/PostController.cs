using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RESTFulSocial.Api.Responses;
using RESTFulSocial.Core.CustomEntities;
using RESTFulSocial.Core.DTOs;
using RESTFulSocial.Core.Entities;
using RESTFulSocial.Core.Interfaces;
using RESTFulSocial.Core.QueryFilters;
using RESTFulSocial.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RESTFulSocial.Api.Controllers
{
    // SOLO USUARIOS AUTENTICADOS
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public PostController(IPostService postservice, IMapper mapper, IUriService uriService)
        {
            _postService = postservice;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Retrive all posts
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetPosts([FromQuery] PostQueryFilter filters)
        {
            var posts = _postService.GetPosts(filters); 
            var postsDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

            var metada = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
                NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
            };

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDtos)
            {
                Meta = metada
            };

            // JsonConvert.SerializeObject(metada), nos devuelve un string en formato Json
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metada));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postDto = _mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            await _postService.InsertPost(post);

            postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = id;

            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
