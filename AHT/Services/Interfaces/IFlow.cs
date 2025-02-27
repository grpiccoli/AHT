﻿using AHT.Models.Entities;
using System;

namespace AHT.Services.Interfaces
{
    public interface IFlow
    {
        string PaymentCreate(int id, string description, int ammount, string email);
        Customer CustomerCreate(int id, string name, string email);
        Register CustomerRegister(string id, Uri returnUrl);
        Customer GetRegisterStatus(string token);
        Customer CustomerCharge(int id, int amount, string subject, string order);
    }
}
