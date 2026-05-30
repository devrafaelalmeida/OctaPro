export function unMask(value: string): string {
  if (!value) return '';
  return value.replace(/[^a-zA-Z0-9]/g, '');
}

export function maskProcessNumber(value: string): string {
  if(value == ''){
    return '';
  }
  let n = value.slice(0, 7); //Número sequencial do processo
  let d = value.slice(7, 9); //Dígito verificador
  let a = value.slice(9, 13); //Ano de ingresso do processo
  let j = value.slice(13, 14); //Órgão do Poder Judiciário
  let t = value.slice(14, 16); //Tribunal de origem
  let u = value.slice(16, 20); // Unidade de origem
  return `${n}-${d}.${a}.${j}.${t}.${u}`;
}

export function maskDataPtBr(value?: string): string {

   if (!value) return '';

  const unmasked = unMask(value);
  const year = unmasked.slice(0, 4);
  const month = unmasked.slice(4, 6);
  const day = unmasked.slice(6, 8);

  const dataFormatada = `${day}/${month}/${year}`;
  return dataFormatada;
}

export function maskDataUSA(value?: string | Date): string {
  if (!value) return '';

  const date = value instanceof Date ? value : new Date(value);

  const result = date.toISOString().split('T')[0];

  return result;
}

export function maskCpfCnpj(value: string | undefined | null): string {
  if (!value) return '';

  const digits = unMask(value);

  // CPF → 11 dígitos
  if (digits.length === 11) {
    return digits.replace(
      /(\d{3})(\d{3})(\d{3})(\d{2})/,
      '$1.$2.$3-$4'
    );
  }

  // CNPJ → 14 dígitos
  if (digits.length === 14) {
    return digits.replace(
      /(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/,
      '$1.$2.$3/$4-$5'
    );
  }


  return digits;
}


