import { FC, useContext, useState } from 'react';
import { context } from '../../App';
import styles from './Home.module.scss';

interface HomeProps { }

const Home: FC<HomeProps> = () => {
  const appContext = useContext(context);
  const [src, setSrc] = useState<string>("");

  const screenshot = () => {
    window.electron.screenshot("VALORANT").then((r: any) => {
      setSrc(r);
    }).catch((e: any) => {
      console.error(e);
    });
  };

  const signout = () => {
    window.electron.signout().then((signedin: boolean) => {
      appContext.setSignedIn(signedin);
    });
  };

  return (
    <div className={styles.Home} data-testid="Home">
      <button className="button" onClick={screenshot}>Screenshot</button>
      <img src={src} />
      <button className="button" onClick={signout}>Sign out</button>
    </div>
  );
}
  ;

export default Home;
