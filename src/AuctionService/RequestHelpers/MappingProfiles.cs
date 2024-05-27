using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Mendaftarkan pemetaan dari Auction ke AuctionDto dan menyertakan properti Item dalam pemetaan tersebut.
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        // Mendaftarkan pemetaan dari Item ke AuctionDto.
        CreateMap<Item, AuctionDto>();
        // Mendaftarkan pemetaan dari CreateAuctionDto ke Auction.
        // Properti Item pada Auction dipetakan dari CreateAuctionDto itu sendiri.
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        // Mendaftarkan pemetaan dari CreateAuctionDto ke Item. 
        CreateMap<CreateAuctionDto, Item>();
    }
}
