import React, { FC } from 'react';
import styles from './FormInput.module.scss';

interface FormInputProps {}

const FormInput: FC<FormInputProps> = () => (
  <div className={styles.FormInput} data-testid="FormInput">
    FormInput Component
  </div>
);

export default FormInput;
