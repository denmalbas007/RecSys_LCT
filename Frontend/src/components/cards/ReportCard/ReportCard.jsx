import cl from "./ReportCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faSliders,
  faTableList,
  faClock,
} from "@fortawesome/free-solid-svg-icons";

export const ReportCard = ({ projectTitle, pending, time }) => {
  return (
    <div className={cl.report_card}>
      <div className={cl.header}>
        <h3>{`Отчет по проекту "${projectTitle}"`}</h3>
        <div className={cl.row}>
          <FontAwesomeIcon icon={faClock} />
          <p>Запрошено в:14:57</p>
        </div>
      </div>
      <div className={cl.body}>
        <div className={cl.body_info}>
          <div className={cl.row}>
            <FontAwesomeIcon icon={faTableList} />
            <p>Записей: 100</p>
          </div>
          <div className={cl.row}>
            <FontAwesomeIcon icon={faSliders} />
            <p>Фильтров: 100</p>
          </div>
        </div>
      </div>
    </div>
  );
};
