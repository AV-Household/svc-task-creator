using System;

namespace TaskCreator.Models;

/// <summary>
/// Запись о члене семьи
/// </summary>
/// <param name="Id">Идентификатор члена семьи</param>
/// <param name="HouseholdId"></param>
/// <param name="Name">Имя</param>
/// <param name="Phone">Номер телефона</param>
/// <param name="EMail"></param>
/// <param name="IsAdult"></param>
public record FamilyMember(
    Guid Id, 
    int HouseholdId, 
    string Name, 
    string Phone, 
    string EMail,
    bool IsAdult);