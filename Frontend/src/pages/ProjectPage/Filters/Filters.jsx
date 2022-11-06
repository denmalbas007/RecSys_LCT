import React, { useState } from "react";
import { useEffect } from "react";
import { doFetchItemsById, doFetchItemsRoot } from "../../../api/Auth";
import { reformatItems } from "../../../api/ItemsParser";
import RadioMenu from "../../../components/buttons/RadioMenu/RadioMenu";
import ItemTreeSelect from "../../../components/dropdowns/ItemTreeSelect/ItemTreeSelect";
import cl from "./Filters.module.scss";

const Filters = () => {
  const [value, setValue] = useState([]);
  const [items, setItems] = useState([]);

  useEffect(() => {
    const fetchItems = async () => {
      const items = await doFetchItemsRoot();
      const reformattedItems = reformatItems(items);
      setItems(reformattedItems);
    };

    fetchItems();
  }, []);

  const fetchNode = async (id) => {
    const items = await doFetchItemsById(id);
    const reformattedItems = reformatItems(items);
    return reformattedItems;
  };

  const onChange = (newValue) => {
    console.log("onChange ", value);
    setValue(newValue);
  };

  return (
    <div className={cl.filters}>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Категория</p>
        <ItemTreeSelect fetchNode={fetchNode} data={items} />
      </div>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Регион</p>
        <ItemTreeSelect data={items} />
      </div>
      <div className={cl.buttons}></div>
    </div>
  );
};

export default Filters;
