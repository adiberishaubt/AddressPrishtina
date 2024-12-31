﻿namespace AddressPrishtina.Models;

public class Address
{
    public int Id { get; set; }
    public string Rruga { get; set; }
    public int Numri { get; set; }
    public bool Approved { get; set; } = false;
    public string UserId { get; set; }
    public User User { get; set; } 
}