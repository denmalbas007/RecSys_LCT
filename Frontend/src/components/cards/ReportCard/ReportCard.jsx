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
        <h4>{`Отчет по проекту "${projectTitle}"`}</h4>
        <div className={cl.header_info}>
          <FontAwesomeIcon icon={faClock} />
          <p>Запрошено в:14:57</p>
        </div>
      </div>
      <div className={cl.body}>
        <div className={cl.body_info}>
          <div className={cl.body_info_item}>
            <FontAwesomeIcon icon={faTableList} />
            <p>Записей:100</p>
          </div>
          <div className={cl.body_info_item}>
            <FontAwesomeIcon icon={faSliders} />
            <p>Фильтров:100</p>
          </div>
        </div>
      </div>
    </div>
  );
};
