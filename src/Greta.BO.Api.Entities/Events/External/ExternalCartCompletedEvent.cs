using System.Text.Json.Serialization;
using MediatR;

namespace Greta.BO.Api.Entities.Events.External;

public class ExternalCartCompletedEvent : INotification
{
    [JsonPropertyName("instanceId")] public string InstansceId { get; set; }

    // Cart ID, generated from customer identity
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("identityType")] public string IdentityType { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
}

public class Address
{

    // Country code.
    [JsonPropertyName("country")] public string Country { get; set; }

    // Subdivision. Usually a state, region, prefecture, or province code, according to ISO 3166-2.
    [JsonPropertyName("subdivision")] public string Subdivision { get; set; }
    [JsonPropertyName("city")] public string City { get; set; }

    // Zip/postal code.
    [JsonPropertyName("postalCode")] public string PostalCode { get; set; }

    // Main address line, usually street and number as free text.
    [JsonPropertyName("addressLine")] public string AddressLine { get; set; }
}

public class ContactDetails
{
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("phone")] public string Phone { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
}

// Customer's billing address
public class BillingAddress
{
    [JsonPropertyName("address")] public Address Address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails ContactDetails { get; set; }
}

// Currency used for pricing in this store
public class Currency
{
    // Currency code
    [JsonPropertyName("code")] public string Code { get; set; }

    // Currency symbol
    [JsonPropertyName("symbol")] public string Symbol { get; set; }
}

// Order totals
public class TotalsCartComplete
{
    // Subtotal of all line items, before tax
    [JsonPropertyName("subtotal")] public int Subtotal { get; set; }

    // Total calculated discount value, according to order.discount
    [JsonPropertyName("discount")] public int Discount { get; set; }

    // Total price
    [JsonPropertyName("total")] public int Total { get; set; }

    // Total line items quantity
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
}

// Selected shipping rule details
public class ShippingRuleDetails
{
    // Selected shipping rule ID
    [JsonPropertyName("ruleId")] public string RuleId { get; set; }

    // Selected option ID
    [JsonPropertyName("optionId")] public string OptionId { get; set; }

    // Rule title (as provided by the store owner)
    [JsonPropertyName("deliveryOption")] public string DeliveryOption { get; set; }
}

public class ShippingAddress
{
    [JsonPropertyName("address")] public Address Address { get; set; }
    [JsonPropertyName("contactDetails")] public ContactDetails ContactDetails { get; set; }
}

// Cart shipping information
public class ShippingInfo
{
    // Selected shipping rule details
    [JsonPropertyName("shippingRuleDetails")] public ShippingRuleDetails ShippingRuleDetails { get; set; }

    // Shipment details when this object describes shipment
    [JsonPropertyName("shippingAddress")] public ShippingAddress ShippingAddress { get; set; }
}