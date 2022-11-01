import cl from "./SignInPage.module.scss";
import logo from "../../assets/logos/logo_full.png";
import { IconInput } from "../../components/inputs/IconInput/IconInput";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faKey } from "@fortawesome/free-solid-svg-icons";
import AccentButton from "../../components/buttons/AccentButton/AccentButton";
import { useContext, useState } from "react";
import { doCheckAuth, doSignIn } from "../../api/Auth";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../../api/AuthContext";

const SignInPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const { onAuthStateChanged } = useContext(AuthContext);
  const navigate = useNavigate();

  const signIn = async (e) => {
    e.preventDefault();
    setLoading(true);
    if (!username || !password) {
      setError("Пожалуйста, заполните все поля");
    } else {
      const request = await doSignIn(username, password);
      if (request.errorMessage) {
        setError(request.errorMessage);
      } else {
        const response = await doCheckAuth();
        onAuthStateChanged(response.user);
        navigate("/");
      }
    }
    setLoading(false);
  };

  return (
    <div className={cl.signin_container}>
      <nav className={cl.logo_container}>
        <img src={logo} alt="logo" />
      </nav>
      <form className={cl.signin} onSubmit={signIn}>
        <h2>Вход в систему</h2>
        <div className={cl.group}>
          <p>Логин</p>
          <IconInput
            onChange={(e) => setUsername(e.target.value)}
            icon={<FontAwesomeIcon icon={faUser} />}
            placeholder="username"
          />
        </div>
        <div className={cl.group}>
          <p>Пароль</p>
          <IconInput
            onChange={(e) => setPassword(e.target.value)}
            icon={<FontAwesomeIcon icon={faKey} />}
            placeholder="********"
            type="password"
          />
        </div>
        <div className={cl.error}>{error}</div>
        <AccentButton disabled={loading}>Войти в систему</AccentButton>
      </form>
    </div>
  );
};

export default SignInPage;
