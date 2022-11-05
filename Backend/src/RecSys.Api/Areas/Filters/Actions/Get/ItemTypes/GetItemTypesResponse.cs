#pragma warning disable IDE0005
using RecSys.Customs.Client;
#pragma warning restore IDE0005

namespace RecSys.Api.Areas.Filters.Actions.Get.ItemTypes;

public record GetItemTypesResponse(ItemType[] ItemTypes);
