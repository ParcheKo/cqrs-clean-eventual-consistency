﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Orders.Api.Endpoints.CreateCard;

public class CreateCardRequest
{
    [Required] public string Number { get; set; }

    [Required] public string CardHolder { get; set; }

    [Required] public DateTime ExpirationDate { get; set; }
}