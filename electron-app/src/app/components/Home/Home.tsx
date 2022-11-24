import React, { FC, useContext } from 'react';
import { context } from '../../../App';
import { SignOut } from '../../services/account';
import styles from './Home.module.scss';

interface HomeProps { }

const Home: FC<HomeProps> = () => {
  const appContext = useContext(context);

  return (
    <div className={styles.Home} data-testid="Home">
      Home Component
      <button className="button" onClick={() => { SignOut(appContext); }}>Sign Out</button>
    </div>
  );
}
  ;

export default Home;
