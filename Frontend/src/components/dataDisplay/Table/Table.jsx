import cl from "./Table.module.scss";

const Table = ({ data }) => {
  return (
    <table className={cl.table}>
      <thead>
        <tr>
          <th>Регион</th>
          <th>Товар</th>
          <th>Страна</th>
          <th>Экспорт</th>
          <th>Импорт</th>
        </tr>
      </thead>
      <tbody>
        {data.length > 0 &&
          data.map((child, index) => (
            <tr key={index}>
              <td>{child.region.name}</td>
              <td>{child.itemType.name}</td>
              <td>{child.country.name}</td>
              <td>
                {child.export.tradeSum}
                {child.export.unit && <span> ({child.export.unit.name})</span>}
              </td>
              <td>
                {child.import.tradeSum}
                {child.import.unit && <span> ({child.import.unit.name})</span>}
              </td>
            </tr>
          ))}
        {data.length == 0 && (
          <tr>
            <td colSpan="5">Нет данных</td>
          </tr>
        )}
      </tbody>
    </table>
  );
};

export default Table;
