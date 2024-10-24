using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<User> _userRepository;

        public CommentController(IRepository<Comment> commentRepository, IRepository<Post> postRepository, IRepository<User> userRepository)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // Create a new comment
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CreateCommentDTO commentRequest)
        {
            // Check if the associated user and post exist
            if (!await EntityExistsAsync(_userRepository, commentRequest.AuthorId) || !await EntityExistsAsync(_postRepository, commentRequest.PostId))
            {
                return BadRequest("User or Post does not exist");
            }

            var newComment = new Comment
            {
                Body = commentRequest.Body,
                AuthorId = commentRequest.AuthorId,
                PostId = commentRequest.PostId
            };

            var createdComment = await _commentRepository.AddAsync(newComment);

            var commentDTO = new CommentDTO
            {
                Id = createdComment.Id,
                Body = createdComment.Body,
                AuthorId = createdComment.AuthorId,
                PostId = createdComment.PostId
            };

            return Created($"/Comments/{commentDTO.Id}", commentDTO);
        }

        // Get a single comment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetSingleComment(int id)
        {
            var comment = await _commentRepository.GetSingleAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                Body = comment.Body,
                AuthorId = comment.AuthorId,
                PostId = comment.PostId
            };

            return Ok(commentDTO);
        }

        // Get all comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetAllComments([FromQuery] int? postId, [FromQuery] int? authorId)
        {
            var comments = await _commentRepository.GetManyAsync();

            if (postId.HasValue)
            {
                comments = comments.Where(c => c.PostId == postId.Value);
            }

            if (authorId.HasValue)
            {
                comments = comments.Where(c => c.AuthorId == authorId.Value);
            }

            var commentDtos = comments.Select(c => new CommentDTO
            {
                Id = c.Id,
                Body = c.Body,
                AuthorId = c.AuthorId,
                PostId = c.PostId
            }).ToList();

            return Ok(commentDtos);
        }

        // Update an existing comment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CreateCommentDTO commentRequest)
        {
            var comment = await _commentRepository.GetSingleAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            comment.Body = commentRequest.Body;

            await _commentRepository.UpdateAsync(comment);

            return NoContent();
        }

        // Delete a comment by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.GetSingleAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
