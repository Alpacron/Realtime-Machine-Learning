import React, { ChangeEvent, Dispatch, FC, SetStateAction, SyntheticEvent } from 'react';
import styles from './FormInput.module.scss';

interface FormInputProps {
  id: string,
  name: string,
  type: "password" | "email" | "text",
  autoComplete: "email" | "password" | "new-password" | "name",
  error: string,
  value: string,
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const FormInput: FC<FormInputProps> = ({ id, name, type, autoComplete, error, value, onChange }: FormInputProps) => (
  <div className={styles.FormInput} data-testid="FormInput">
    <label htmlFor={id}>{name}:</label>
    <input type={type} id={id} spellCheck="false" autoComplete={autoComplete} data-testid={id} value={value} onChange={onChange} />
    <label htmlFor={id} className="error">{error}</label>
  </div>
);

export default FormInput;
