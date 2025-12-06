using UnoBookRail.Common.Auth;

namespace UnoBookRail.Common.Tickets;

public class TicketBooking
{
    public static Ticket BookPricingOption(PricingOption option, User user, string startStation, string endStation) =>
        new(option.OptionType, user.Identifier, option.Price, DateTime.UtcNow + "secret", startStation, endStation);
}