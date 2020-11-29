using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlaystationWishlist.AutoMapper
{
    public class PlaystationGameProfile : Profile
    {
        public PlaystationGameProfile()
        {
            Regex currencyRegex = new Regex(@".+?(?= ?\d)");
            Regex priceRegex = new Regex(@"\d+[,.]?\d+");

            CreateMap<Core.Entities.PlaystationGame, DataAccess.Models.PlaystationGame>()
                .ForMember(dest => dest.Currency,
                    opt => opt.MapFrom(src =>
                        src.FinalPrice != null
                            ? (src.FinalPrice.Any(char.IsDigit) ? currencyRegex.Match(src.FinalPrice).Value : null)
                            : null))
                .ForMember(dest => dest.FinalPrice,
                    opt => opt.MapFrom(src =>
                        src.FinalPrice != null
                            ? (src.FinalPrice.Any(char.IsDigit)
                                ? (src.FinalPrice != null
                                    ? (decimal?) decimal.Parse(priceRegex.Match(src.FinalPrice).Value,
                                        new CultureInfo(src.Region))
                                    : null)
                                : 0)
                            : 0))
                .ForMember(dest => dest.OriginalPrice,
                    opt => opt.MapFrom(src =>
                        src.OriginalPrice != null
                            ? (src.OriginalPrice.Any(char.IsDigit)
                                ? (src.OriginalPrice != null
                                    ? (decimal?) decimal.Parse(priceRegex.Match(src.OriginalPrice).Value,
                                        new CultureInfo(src.Region))
                                    : null)
                                : null)
                            : null))
                .ReverseMap();

            CreateMap<PlaystationWishlistWebSite.Models.GamesViewModel, Core.Entities.PlaystationGame>()
                .ReverseMap();
        }
    }
}
