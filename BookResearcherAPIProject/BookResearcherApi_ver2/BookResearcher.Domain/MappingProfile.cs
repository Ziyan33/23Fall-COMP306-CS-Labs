using AutoMapper;
using BookResearcher.Domain.Models;
using BookResearcher.Domain.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Domain to DTO
        CreateMap<Author, AuthorDTO>();
        CreateMap<Book, BookDTO>();
        CreateMap<BookAuthors, BookAuthorsDTO>();
        CreateMap<BookAvailability, BookAvailabilityDTO>();
        CreateMap<LibraryBranch, LibraryBranchDTO>();

        CreateMap<Author, AuthorPatchDTO>().ReverseMap();
        // DTO to Domain (if you need to support updating/creating entities from DTOs)
        CreateMap<AuthorDTO, Author>();
        
        
        // CreateMap<BookDTO, Book>();
        // AutoMapper configuration
        CreateMap<BookDTO, Book>()
            .ForMember(dest => dest.BookID, opt => opt.Ignore()); // Ignore BookID during mapping

        CreateMap<BookAuthorsDTO, BookAuthors>();

        CreateMap<BookAvailabilityDTO, BookAvailability>();
        // AutoMapper configuration

        CreateMap<LibraryBranchDTO, LibraryBranch>()
            .ForMember(dest => dest.LibraryID, opt => opt.Ignore()); // Ignore LibraryID during mapping

    }
}
