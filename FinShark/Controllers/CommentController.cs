﻿using FinShark.Data;
using FinShark.DTOS.Comment;
using FinShark.Interfaces;
using FinShark.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly ApplicationDBContext _dbContext;
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;

    public CommentController(ApplicationDBContext context,
                            ICommentRepository commentRepository,
                            IStockRepository stockRepository)
    {
        _dbContext = context;
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentRepository.GetAllAsync();
        var commentDto = comments.Select(s => s.ToCommentDto());

        return Ok(commentDto);
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null) { return NotFound(); }
        return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
        if (!await _stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
        };
        var commentModel = commentDto.ToCommentFromCreate(stockId);
        await _commentRepository.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());

    }
}