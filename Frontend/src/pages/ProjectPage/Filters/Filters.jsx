import React, { useState } from "react";
import { useEffect } from "react";
import {
  doFetchItemsById,
  doFetchItemsRoot,
  doFetchCountries,
  doFetchRegions,
} from "../../../api/Auth";
import { reformatItems } from "../../../api/ItemsParser";
import AccentButton from "../../../components/buttons/AccentButton/AccentButton";
import ItemTreeSelect from "../../../components/dropdowns/ItemTreeSelect/ItemTreeSelect";
import cl from "./Filters.module.scss";

const Filters = ({ filters, project, onProjectSave }) => {
  const [countries, setCountries] = useState([]);
  const [selectedCountries, setSelectedCountries] = useState([]);
  const [items, setItems] = useState([]);
  const [selectedItems, setSelectedItems] = useState([]);
  const [regions, setRegions] = useState([]);
  const [selectedRegions, setSelectedRegions] = useState([]);

  const generateNewReport = (e) => {
    console.log(selectedCountries);
    console.log(selectedItems);
  };

  const saveProject = () => {
    onProjectSave({
      countries: selectedCountries,
      items: selectedItems,
      regions: selectedRegions,
    });
  };

  // combobox contents
  useEffect(() => {
    const fetchItems = async () => {
      const items = await doFetchItemsRoot();
      const reformattedItems = reformatItems(items);
      setItems(reformattedItems);
    };

    const fetchCountries = async () => {
      const countries = await doFetchCountries();
      // change name to label
      const reformattedCountries = countries.map((country) => {
        return {
          ...country,
          label: country.name,
          checked: project.filter.countries.includes(country.id),
        };
      });
      setCountries(reformattedCountries);
    };

    const fetchRegions = async () => {
      const regions = await doFetchRegions();
      // change name to label
      const reformattedRegions = regions.map((region) => {
        return {
          ...region,
          label: region.name,
        };
      });
      setRegions(reformattedRegions);
    };

    fetchItems();
    fetchCountries();
    fetchRegions();
  }, []);

  const fetchNode = async (id) => {
    const items = await doFetchItemsById(id);
    const reformattedItems = reformatItems(items);
    return reformattedItems;
  };

  return (
    <div className={cl.filters}>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Категория</p>
        <ItemTreeSelect
          fetchNode={fetchNode}
          data={items}
          onSelectChange={setSelectedItems}
        />
      </div>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Страна</p>
        <ItemTreeSelect
          data={countries}
          onSelectChange={setSelectedCountries}
        />
      </div>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Регион РФ</p>
        <ItemTreeSelect data={regions} onSelectChange={setSelectedRegions} />
      </div>
      <div className={cl.buttons}>
        <AccentButton onClick={saveProject}>Сохранить проект</AccentButton>
        <AccentButton onClick={generateNewReport} secondary>
          Создать отчёт
        </AccentButton>
      </div>
    </div>
  );
};

export default Filters;
