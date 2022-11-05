const testJson = {
  itemTypes: [
    {
      hasChildNodes: true,
      id: "01",
      name: "ШТ-ЖИВЫЕ ЖИВОТНЫЕ",
    },
    {
      hasChildNodes: true,
      id: "02",
      name: "МЯСО И ПИЩЕВЫЕ МЯСНЫЕ СУБПРОДУКТЫ",
    },
  ],
};

export const reformatItems = (json) => {
  // loop through the itemTypes and rename name to label
  const itemTypes = json.map((itemType) => {
    if (itemType.hasChildNodes) {
      return {
        ...itemType,
        children: [
          {
            label: "Загрузка...",
            disabled: true,
          },
        ],
      };
    }
    return itemType;
  });

  return itemTypes;
};
