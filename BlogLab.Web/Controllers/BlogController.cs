using BlogLab.Models.Blog;
using BlogLab.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPhotoRepository _photoRepository;

        public BlogController(IBlogRepository blogRepository, 
            IPhotoRepository photoRepository)
        {
            _blogRepository = blogRepository;
            _photoRepository = photoRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Blog>> Create(BlogCreate blogCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            if (blogCreate.PhotoId.HasValue)
            {
                var photo = await _photoRepository.GetAsync(blogCreate.PhotoId.Value);
                if (photo.ApplicationUserid != applicationUserId)
                {
                    return BadRequest("User does not own photo");
                }
            }
            var blog = await _blogRepository.UpsertAsync(blogCreate, applicationUserId);
            return Ok(blog);
        }

        //http://localhost:5000/api/Blog/?PageSize=10&Param2=12
        [HttpGet]
        public async Task<ActionResult<PageResults<Blog>>> GetAll([FromQuery] BlogPaging blogPaging)
        {
            var blogs = await _blogRepository.GetAllAsync(blogPaging);
            return Ok(blogs);
        }

        [HttpGet("{blogId}")]
        public async Task<ActionResult<Blog>> Get(int blogId)
        {
            var blog = await _blogRepository.GetAsync(blogId);
            return Ok(blog);
        }

        //http://localhost:5000/api/Blog/user/12
        [HttpGet("user/{applicationUserId}")]
        public async Task<ActionResult<List<Blog>>> GetByApplicationUserId(int applicationUserId)
        {
            var blogs = await _blogRepository.GetAllByUserIdAsync(applicationUserId);
            return Ok(blogs);
        }

        //http://localhost:5000/api/Blog/user/12
        [HttpGet("famous")]
        public async Task<ActionResult<List<Blog>>> GetAllFamous()
        {
            var blogs = await _blogRepository.GetAllFamousAsync();
            return Ok(blogs);
        }

        //http://localhost:5000/api/Blog/user/12
        [Authorize]
        [HttpDelete("{blogId}")]
        public async Task<ActionResult<int>> Delete(int blogId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var foundBlog = await _blogRepository.GetAsync(blogId);
            if (foundBlog == null) return BadRequest("Blog does not exist");
            if (foundBlog.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _blogRepository.DeleteAsync(blogId);
                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("You did not create this log");
            }
        }

    }
}
