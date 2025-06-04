﻿using Microsoft.AspNetCore.Identity;

namespace SuggestionHub.Infrastructure.Identity.Errors;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() =>
        new IdentityError { Code = nameof(DefaultError), Description = "Ocorreu um erro desconhecido." };

    public override IdentityError ConcurrencyFailure() =>
        new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Falha de simultaneidade otimista, o objeto foi modificado." };

    public override IdentityError PasswordMismatch() =>
        new IdentityError { Code = nameof(PasswordMismatch), Description = "Senha incorreta." };

    public override IdentityError InvalidToken() =>
        new IdentityError { Code = nameof(InvalidToken), Description = "Token inválido." };

    public override IdentityError LoginAlreadyAssociated() =>
        new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Já existe um usuário com este login." };

    public override IdentityError InvalidUserName(string userName) =>
        new IdentityError { Code = nameof(InvalidUserName), Description = $"Nome de usuário '{userName}' é inválido. Apenas letras ou dígitos são permitidos." };

    public override IdentityError InvalidEmail(string email) =>
        new IdentityError { Code = nameof(InvalidEmail), Description = $"O e-mail '{email}' é inválido." };

    public override IdentityError DuplicateUserName(string userName) =>
        new IdentityError { Code = nameof(DuplicateUserName), Description = $"O nome de usuário '{userName}' já está em uso." };

    public override IdentityError DuplicateEmail(string email) =>
        new IdentityError { Code = nameof(DuplicateEmail), Description = $"O e-mail '{email}' já está em uso." };

    public override IdentityError InvalidRoleName(string role) =>
        new IdentityError { Code = nameof(InvalidRoleName), Description = $"O nome da função '{role}' é inválido." };

    public override IdentityError DuplicateRoleName(string role) =>
        new IdentityError { Code = nameof(DuplicateRoleName), Description = $"A função '{role}' já existe." };

    public override IdentityError UserAlreadyHasPassword() =>
        new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "O usuário já possui uma senha definida." };

    public override IdentityError UserLockoutNotEnabled() =>
        new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "O bloqueio não está habilitado para este usuário." };

    public override IdentityError UserAlreadyInRole(string role) =>
        new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"O usuário já pertence à função '{role}'." };

    public override IdentityError UserNotInRole(string role) =>
        new IdentityError { Code = nameof(UserNotInRole), Description = $"O usuário não pertence à função '{role}'." };

    public override IdentityError PasswordTooShort(int length) =>
        new IdentityError { Code = nameof(PasswordTooShort), Description = $"A senha deve conter pelo menos {length} caracteres." };

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter pelo menos um caractere não alfanumérico." };

    public override IdentityError PasswordRequiresDigit() =>
        new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter pelo menos um número ('0'-'9')." };

    public override IdentityError PasswordRequiresLower() =>
        new IdentityError { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter pelo menos uma letra minúscula ('a'-'z')." };

    public override IdentityError PasswordRequiresUpper() =>
        new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter pelo menos uma letra maiúscula ('A'-'Z')." };

    public override IdentityError RecoveryCodeRedemptionFailed() =>
        new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = "O código de recuperação foi usado incorretamente." };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) =>
        new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"A senha deve conter pelo menos {uniqueChars} caracteres únicos." };
}
