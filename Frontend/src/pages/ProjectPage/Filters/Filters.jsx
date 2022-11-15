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

const Filters = ({ project, onProjectSave, onReportCreate, projectSaving }) => {
  const [countries, setCountries] = useState([]);
  const [items, setItems] = useState([]);
  const [regions, setRegions] = useState([]);
  const [selectedCountries, setSelectedCountries] = useState([]);
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectedRegions, setSelectedRegions] = useState([]);

  const createNewReport = (e) => {
    onReportCreate({
      countries: selectedCountries,
      items: selectedItems,
      regions: selectedRegions,
    });
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
      // ----------selected items are not yet supported
      const items = await doFetchItemsRoot();
      const reformattedItems = reformatItems(items);
      setItems(reformattedItems);
    };

    const fetchCountries = async () => {
      const countries = await doFetchCountries();
      const reformattedCountries = countries.map((country) => {
        // update selected countries
        if (project.filter.countries.includes(country.id)) {
          setSelectedCountries((prev) => [...prev, country]);
        }
        // change name to label
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
      const reformattedRegions = regions.map((region) => {
        // update selected regions
        if (project.filter.regions.includes(region.id)) {
          setSelectedRegions((prev) => [...prev, region]);
        }
        // change name to label
        return {
          ...region,
          label: region.name,
          checked: project.filter.regions.includes(region.id),
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
        <AccentButton disabled={projectSaving} onClick={saveProject}>
          Сохранить проект
        </AccentButton>
        <AccentButton onClick={createNewReport} secondary>
          Создать отчёт
        </AccentButton>
      </div>
    </div>
  );
};

export default Filters;
