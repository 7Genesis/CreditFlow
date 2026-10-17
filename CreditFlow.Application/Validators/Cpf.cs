namespace CreditFlow.Application.Validators;

public static class Cpf
{
    // Valida os 11 dígitos pelos dígitos verificadores. Recusa sequências repetidas (111.111.111-11).
    public static bool EhValido(string? cpf)
    {
        if (cpf is null || cpf.Length != 11 || !cpf.All(char.IsAsciiDigit))
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        return cpf[9] - '0' == CalcularDigito(cpf, 9)
            && cpf[10] - '0' == CalcularDigito(cpf, 10);
    }

    private static int CalcularDigito(string cpf, int tamanho)
    {
        var soma = 0;
        for (var i = 0; i < tamanho; i++)
            soma += (cpf[i] - '0') * (tamanho + 1 - i);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
