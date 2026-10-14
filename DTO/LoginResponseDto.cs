namespace SavelevCrossPlatform.Dtos;

public record LoginResponseDto(string Token, string Role, DateTime ExpiresAt);
