import React, { FC, useContext, useEffect, useState } from 'react';
import { context } from '../../../App';
import { SignOut } from '../../services/account';
import styles from './Home.module.scss';

interface HomeProps { }

const Home: FC<HomeProps> = () => {
  const appContext = useContext(context);
  const [src, setSrc] = useState<string>("");

  useEffect(() => {
    (window as any).electronAPI.screenshot("VALORANT").then((r: any) => {
      console.log(r);
      setSrc(r);
    }).catch((e: any) => {
      console.error(e);
    });
  }, []);

  return (
    <div className={styles.Home} data-testid="Home">
      <img src={src} />
      <button className="button" onClick={() => { SignOut(appContext); }}>Sign Out</button>
    </div>
  );
}
  ;

export default Home;
