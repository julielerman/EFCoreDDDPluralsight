public readonly record struct SpecificationSet
    (int AdvanceAmountUSD, 
    int HardCoverRoyaltyPct,
    int SoftCoverRoyaltyPct, int DigitalRoyaltyPct,
    int TranslationRoyaltyUSD, bool PublicityProvided,
    bool AuthorAvailableForPR, int PromoCopiesForAuthor,
    decimal PriceForAddlAuthorCopiesUSD);


//   Defined as a positional record using a primary constructor

//   record struct is a value type
//   positional record struct is NOT IMMUTABLE 
//   positional readonly record struct is also immutable and the right choice here

//   record (alone) is a reference type
//   and FYI, positional record is immutable by default
