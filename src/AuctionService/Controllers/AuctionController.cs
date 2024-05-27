using AuctionService.Data;
using AuctionService.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var res = await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(res);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid Id)
    {
        var res = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (res == null) return NotFound();

        return _mapper.Map<AuctionDto>(res);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);
        // TODO: add current user as seller
        auction.Seller = "ahmad";

        _context.Auctions.Add(auction);

        var res = await _context.SaveChangesAsync() > 0;

        if (!res) return BadRequest("Could not save data");

        return CreatedAtAction(nameof(GetAuctionById),
            new { auction.Id }, _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("Id")]
    public async Task<ActionResult<AuctionDto>> UpdateAuction(Guid Id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _context.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (auction == null) return NotFound();

        //TODO : Check seller == username

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;

        var res = await _context.SaveChangesAsync() > 0;

        if (res) return Ok();

        return BadRequest();

    }

    [HttpDelete("Id")]
    public async Task<ActionResult> DeleteAuction(Guid Id)
    {
        var auction = await _context.Auctions
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (auction == null) return NotFound();

        _context.Remove(auction);

        var res = await _context.SaveChangesAsync() > 0;

        if (!res) return BadRequest("Can't update Db");

        return Ok();
    }
}
