import cl from "./Table.module.scss";

const tableData = {
  headers: [
    { id: "name", label: "Name" },
    { id: "age", label: "Age" },
    { id: "country", label: "Country" },
  ],
  rows: [
    {
      id: 1,
      name: "JohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohn",
      age: 20,
      country: "USA",
    },
    { id: 2, name: "Jane", age: 21, country: "Canada" },
  ],
};

const Table = ({ data }) => {
  if (!data) {
    data = tableData;
  }
  return (
    <table className={cl.table}>
      <thead>
        <tr>
          {data.headers.map((header) => (
            <th key={header.id}>{header.label}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {data.rows.map((row) => (
          <tr key={row.id}>
            {data.headers.map((header) => (
              <td key={header.id}>{row[header.id]}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default Table;
