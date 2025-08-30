﻿namespace OrderMicroservices.Common.ValueObjects
{
    public record Address(
        string Street,
        string Number,
        string Complement,
        string City,
        string State,
        string ZipCode,
        string Country
        )
    {
    }
}
