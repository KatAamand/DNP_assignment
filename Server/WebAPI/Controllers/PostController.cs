﻿using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;

    public PostController(IRepository<Post> postRepository, IRepository<User> userRepository,
        IRepository<Comment> commentRepository)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    }
    
    // Create new post
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDTO postRequest)
    {
        if (!await EntityExistsAsync(_userRepository, postRequest.AuthorId))
        {
            return BadRequest("User does not exist");
        }

        var newPost = new Post(postRequest.Title, postRequest.AuthorId, postRequest.Body); 
        var createdPost = await _postRepository.AddAsync(newPost);

        var postDTO = new PostDTO
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            AuthorId = createdPost.AuthorId,
            Created = createdPost.Created,
            NoOfVotes = createdPost.NoOfVotes
        }; 
        
        return Created($"/Posts/{postDTO.Id}", createdPost);
    }
    
    // Get a single post
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetSinglePost(int id)
    {
        var post = await _postRepository.GetSingleAsync(id);
        
        if (post == null)
        {
            return NotFound();
        }

        var postDto = new PostDTO
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            Title = post.Title,
            Body = post.Body,
            NoOfVotes = post.NoOfVotes,
            Created = post.Created
        }; 
        
        return Ok(postDto);
    }
    
    // Get multiple posts with search parameters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPosts([FromQuery] string? title,
        [FromQuery] int? authorId, [FromQuery] DateOnly? created)
    {
        var posts = await _postRepository.GetManyAsync();

        if (!string.IsNullOrEmpty(title))
        {
            posts = posts.Where(p => p.Title.Contains(title));
        }

        if (authorId.HasValue)
        {
            posts = posts.Where(p => p.AuthorId == authorId);
        }

        if (created.HasValue)
        {
            posts = posts.Where(p => p.Created == created.Value);
        }

        var postDtos = posts.Select(p => new PostDTO
        {
            Id = p.Id, AuthorId = p.AuthorId, Created = p.Created, NoOfVotes = p.NoOfVotes, Title = p.Title,
            Body = p.Body
        }).ToList();
        
        return Ok(postDtos);
    }
    
    // Update existing post
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] CreatePostDTO postRequest) 
    {
        var post = await _postRepository.GetSingleAsync(id);
        if (post is null)
        {
            return NotFound();
        }
        
        post.Title = postRequest.Title;
        post.Body = postRequest.Body;
        post.NoOfVotes = post.NoOfVotes;
        
        await _postRepository.UpdateAsync(post);
        
        return NoContent();
    }
    
    // Delete single post
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _postRepository.GetSingleAsync(id);
        if (post is null)
        {
            return NotFound();
        }
        
        await _postRepository.DeleteAsync(id);

        return NoContent();
    }
    
}